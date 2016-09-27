#!/usr/bin/env python
import sys
import pika

connection = pika.BlockingConnection(pika.ConnectionParameters(host='localhost'))
channel = connection.channel()

channel.queue_declare(queue='hello')

print("Type a message. Type 'quit' to quit.")
message = ''

while (message != 'quit'):
    message = input('> ')
    channel.basic_publish(exchange='', routing_key='hello', body=message)    
    print("  Sent: {0}".format(message))

print("Bye!")
connection.close()