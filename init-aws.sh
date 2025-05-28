#!/bin/bash
echo "Creating SQS queues..."
awslocal --region ap-southeast-1 sqs create-queue --queue-name booking
awslocal --region ap-southeast-1 sqs create-queue --queue-name property
awslocal --region ap-southeast-1 sqs create-queue --queue-name payment
echo "SQS queues created!"

echo "Creating DynamoDB tables..."
awslocal --region ap-southeast-1 dynamodb create-table \
  --table-name PropertyEvents \
  --attribute-definitions AttributeName=AggregateId,AttributeType=S AttributeName=Version,AttributeType=N \
  --key-schema AttributeName=AggregateId,KeyType=HASH AttributeName=Version,KeyType=RANGE \
  --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5

awslocal --region ap-southeast-1 dynamodb create-table \
  --table-name BookingEvents \
  --attribute-definitions AttributeName=AggregateId,AttributeType=S AttributeName=Version,AttributeType=N \
  --key-schema AttributeName=AggregateId,KeyType=HASH AttributeName=Version,KeyType=RANGE \
  --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5

awslocal --region ap-southeast-1 dynamodb create-table \
  --table-name PaymentEvents \
  --attribute-definitions AttributeName=AggregateId,AttributeType=S AttributeName=Version,AttributeType=N \
  --key-schema AttributeName=AggregateId,KeyType=HASH AttributeName=Version,KeyType=RANGE \
  --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5
echo "DynamoDB tables created!"
