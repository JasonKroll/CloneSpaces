# CloneSpaces

Clone DigitalOcean Spaces or Amazon S3 from one Space or Bucket to another

## Description

A very simple tool to copy all files from DO Spaces or Amazon S3 to another location and replicate all Keys.

## Getting Started

### Dependencies

- net5.0
- AWSSDK.S3
- System.CommandLine

### Executing program

- Clone the repo

### Run for DO Spaces

```
export SPACES_KEY=<your key>
export SPACES_SECRET=<your secret>
```

```
dotnet run -- <source-space-name> <destination-space-name>
```

### Run for S3

```
export AWS_ACCESS_KEY_ID=<your key>
export AWS_ACCESS_KEY_ID=<your secret>
```

```
dotnet run -- <source-space-name> <destination-space-name> --s3
```

## Use Docker

```
sh run.sh <key> <secret> <source> <destination>
```

## Help

Haven't tested on S3 yet. Most likely the EndpointURL config needs work.

## License

The MIT License (MIT)

Copyright (c) 2015 Chris Kibble

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
