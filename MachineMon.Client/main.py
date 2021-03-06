#!/usr/bin/env python
import datetime
import io
import json
import os
import pika
import socket
import sys
import time
import uuid

from metrics.free_disk_space import FreeDiskSpaceMetric

class MachineMonClient:

    CONNECT_RETRY_INTERVAL_IN_SECONDS = 5
    SEND_INTERVAL_IN_SECONDS = 60
    QUEUE_NAME = 'machinemon'
    UUID_FILENAME = 'uuid.txt'

    """Starts a new instance of the MachineMon client, which periodically reports metrics."""
    def start(self):

        self.load_or_generate_host_id()
        config = self.load_config()

        server_hostname = config["hostname"]
        credentials = pika.PlainCredentials(config["username"], config["password"])
        
        print("Connecting to {0} ...".format(server_hostname, credentials))
        connection = self.connect_to_server(server_hostname, credentials)
        channel = connection.channel()
        channel.queue_declare(queue=self.QUEUE_NAME)

        print("Connected. Sending messages every {0} minutes".format(self.SEND_INTERVAL_IN_SECONDS // 60))

        while (True):
            data = FreeDiskSpaceMetric().get_metric()
            self.add_required_fields(data)

            try:
                channel.basic_publish(exchange='', routing_key=self.QUEUE_NAME, body = json.dumps(data))
            except:
                print("Connection to server lost. Reconnecting.")
                connection = self.connect_to_server(server_hostname, credentials)
                channel = connection.channel()
                channel.queue_declare(queue=self.QUEUE_NAME)
                print("Connected.")
                
            time.sleep(self.SEND_INTERVAL_IN_SECONDS)

        connection.close()

    def add_required_fields(self, data):
        data["MessageDateTimeUtc"] = datetime.datetime.utcnow().isoformat()
        data["Sender"] = self.host_id.hex


    # Open config.json as JSON and get the host name, user name, and 
    def load_config(self):
        config_file = open('config.json', 'r')
        text = config_file.read().strip()
        config_file.close
        config = json.loads(text)
        return config
    
    # Loads this cilent's unique GUID/UUID. If it doesn't have one, generate+persist one.
    def load_or_generate_host_id(self):        
        if (os.path.isfile(self.UUID_FILENAME)):
            f = open(self.UUID_FILENAME, 'r')
            hex = f.read().strip()
            self.host_id = uuid.UUID(hex)
            f.close()            
        else:
            self.host_id = uuid.uuid4()
            f = open(self.UUID_FILENAME, 'w')
            f.write(self.host_id.hex)
            f.close()

    def connect_to_server(self, server_hostname, credentials):
        connected = False
        tried = 0
        
        while not connected:
            try:
                tried += 1
                connection = pika.BlockingConnection(pika.ConnectionParameters(host=server_hostname, credentials=credentials))
                connected = True
            except:            
                print("Connection failed. Retry #{0} ...".format(tried))
                time.sleep(MachineMonClient.CONNECT_RETRY_INTERVAL_IN_SECONDS)
        
        return connection

MachineMonClient().start()