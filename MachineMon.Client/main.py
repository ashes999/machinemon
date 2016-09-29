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

    def create_json(self, message):
        data = {}
        data["Contents"] = message
        data["Sender"] = socket.gethostname()
        data["MessageDateTimeUtc"] = datetime.datetime.utcnow().isoformat()
        return json.dumps(data)

    def start(self, host_name):
        print("Connecting to {0} ...".format(host_name))
        credentials = pika.PlainCredentials('machinemon', 'machinemon')
        connection = pika.BlockingConnection(pika.ConnectionParameters(host=host_name, credentials=credentials))
        channel = connection.channel()
        channel.queue_declare(queue='hello')

        print("Connected. Sending messages every {0} minutes".format(self.SEND_INTERVAL_IN_SECONDS // 60))

        while (True):
            message = FreeDiskSpaceMetric().get_metric()["message"]
            channel.basic_publish(exchange='', routing_key='hello', body = self.create_json(message))    
            time.sleep(self.SEND_INTERVAL_IN_SECONDS)

        connection.close()


config = open('config.txt', 'r')
host_name = config.read().strip()
config.close
MachineMonClient().start(host_name)