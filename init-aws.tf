provider "aws" {
  region                      = "ap-southeast-1"
  access_key                  = "test"
  secret_access_key           = "test"
  s3_force_path_style         = true
  skip_credentials_validation = true
  skip_metadata_api_check     = true
  skip_requesting_account_id  = true
  endpoints {
    sqs       = "http://localhost:4566"
    dynamodb  = "http://localhost:4566"
  }
}

resource "aws_sqs_queue" "booking" {
  name = "booking"
}

resource "aws_sqs_queue" "property" {
  name = "property"
}

resource "aws_sqs_queue" "payment" {
  name = "payment"
}

resource "aws_dynamodb_table" "property_events" {
  name         = "PropertyEvents"
  billing_mode = "PROVISIONED"

  read_capacity  = 5
  write_capacity = 5

  hash_key  = "AggregateId"
  range_key = "Version"

  attribute {
    name = "AggregateId"
    type = "S"
  }

  attribute {
    name = "Version"
    type = "N"
  }
}

resource "aws_dynamodb_table" "booking_events" {
  name         = "BookingEvents"
  billing_mode = "PROVISIONED"

  read_capacity  = 5
  write_capacity = 5

  hash_key  = "AggregateId"
  range_key = "Version"

  attribute {
    name = "AggregateId"
    type = "S"
  }

  attribute {
    name = "Version"
    type = "N"
  }
}

resource "aws_dynamodb_table" "payment_events" {
  name         = "PaymentEvents"
  billing_mode = "PROVISIONED"

  read_capacity  = 5
  write_capacity = 5

  hash_key  = "AggregateId"
  range_key = "Version"

  attribute {
    name = "AggregateId"
    type = "S"
  }

  attribute {
    name = "Version"
    type = "N"
  }
}
