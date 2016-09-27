#!/usr/bin/env python
import datetime
import json
import pika
import socket
import sys

def create_json(message):
    data = {}
    data["Contents"] = message
    data["Sender"] = socket.gethostname()
    data["MessageDateTimeUtc"] = datetime.datetime.utcnow().isoformat()
    return json.dumps(data)

def main_loop():
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='localhost'))
    channel = connection.channel()

    channel.queue_declare(queue='hello')

    print("Type a message. Type 'quit' to quit.")
    message = ''

    while (message != 'quit'):
        message = input('> ')
        if message != 'quit':
            channel.basic_publish(exchange='', routing_key='hello', body = create_json(message))    
            print("  Sent: {0}".format(message))

    print("Bye!")
    connection.close()

main_loop()