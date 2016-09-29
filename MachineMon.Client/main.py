#!/usr/bin/env python
import datetime
import io
import json
import pika
import socket
import time

from metrics.free_disk_space import FreeDiskSpaceMetric

class MachineMonClient:

    SEND_INTERVAL_IN_SECONDS = 60
    
    def load_config(self):
        # Open config.json as JSON and get the host name, user name, and password
        config_file = open('config.json', 'r')
        text = config_file.read().strip()
        config_file.close
        config = json.loads(text)
        return config

    def start(self):
        config = self.load_config()
        hostname = config["hostname"]
        print("Connecting to {0} ...".format(hostname))
        credentials = pika.PlainCredentials(config["username"], config["password"])
        connection = pika.BlockingConnection(pika.ConnectionParameters(host=hostname, credentials=credentials))
        channel = connection.channel()
        channel.queue_declare(queue='hello')

        print("Connected. Sending messages every {0} minutes".format(self.SEND_INTERVAL_IN_SECONDS // 60))

        while (True):
            data = FreeDiskSpaceMetric().get_metric()
            self.add_required_fields(data)
            channel.basic_publish(exchange='', routing_key='hello', body = json.dumps(data))
            time.sleep(self.SEND_INTERVAL_IN_SECONDS)

        connection.close()

    def add_required_fields(self, data):
        data["Sender"] = socket.gethostname()
        data["MessageDateTimeUtc"] = datetime.datetime.utcnow().isoformat()

MachineMonClient().start()