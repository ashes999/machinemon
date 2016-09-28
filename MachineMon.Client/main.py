#!/usr/bin/env python
import datetime
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

    def start(self):
        connection = pika.BlockingConnection(pika.ConnectionParameters(host='localhost'))
        channel = connection.channel()

        channel.queue_declare(queue='hello')

        while (True):
            message = FreeDiskSpaceMetric().get_metric()["message"]
            channel.basic_publish(exchange='', routing_key='hello', body = self.create_json(message))    
            time.sleep(self.SEND_INTERVAL_IN_SECONDS)

        connection.close()

MachineMonClient().start()