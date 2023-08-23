# DoxieServer

This Docker container accepts scans as they are sent by a Doxie scanner and forwards them to a SFTP server. It can send them as the original JPEG file provided by the scanner, or wrap the image in a PDF file, or both.

## Docker Image

This software is available on the Docker Hub: [nicowelles/doxieserver](https://hub.docker.com/r/nicowelles/doxieserver). Currently there is only the latest tag, since the software is already basically feature complete and I do not intend to add breaking changes.

## Environment Variables

The software needs certain environment variables to work. **Please provide all variables in your docker-compose file!**

| Variable Name   | Description                                                                          | Example Value                 |
|-----------------|--------------------------------------------------------------------------------------|-------------------------------|
| ASPNETCORE_URLS | The internal port the server listens on. This should stay at the default value.      | 'http://*:80'                 |
| PDF_ENABLED     | If the scanned document should be transformed to a PDF and sent to the SFTP server.  | 'false'                       |
| IMAGE_ENABLED   | If the scanned document should be sent to the server in its original JPEG format.    | 'true'                        |
| USERNAME        | The username your Doxie needs to provide to be able to send documents to the server. | 'Doxie'                       |
| PASSWORD        | The password your Doxie needs to provide to be able to send documents to the server. | 'your-secret-password'        |
| SFTP_HOST       | The host name of the SFTP server the document will be sent to.                       | '192.168.178.99'              |
| SFTP_PORT       | The port of the SFTP server the document will be sent to.                            | '22'                          |
| SFTP_USERNAME   | The username for the SFTP server the document will be sent to.                       | 'Doxie'                       |
| SFTP_PASSWORD   | The password for the SFTP server the document will be sent to.                       | 'your-even-secreter-password' |
| TARGET_PATH     | The relative target path on the SFTP server that the documents will be sent to.      | '/paperless/consume'          |

## Example docker-compose.yml

```
services:
  doxieserver:
    image: nicowelles/doxieserver:latest
    restart: 'always'
    ports:
      - 80:80
    environment:
      ASPNETCORE_URLS: 'http://*:80'
      PDF_ENABLED: 'false'
      IMAGE_ENABLED: 'true'
      USERNAME: 'Doxie'
      PASSWORD: 'example_password_123'
      SFTP_HOST: '192.168.178.99'
      SFTP_PORT: '223'
      SFTP_USERNAME: 'sftp_user'
      SFTP_PASSWORD: 'sftp_user_password_123'
      TARGET_PATH: '/Scans'
```

## Doxie Configuration

This is an example configuration that you need to set in your Doxie scanner.
Most importantly:
* The URL must be in the format `http://<docker hostname>/documents/post`
* It is recommended to use a reverse proxy, since Doxie seems to have a problem with sending documents to a URL that contains a port number.
* Sending documents via HTTPS does not seem to be supported on Doxie's side. I tried to get it to work, but could not figure it out. This means the _doxieserver_ container must be accessible via HTTP.
* Username and password must be set to the `USERNAME` and `PASSWORD` environment variables you set in docker-compose.
* File Parameter Name must be set to `document` and the optional parameters must be left empty.

![](/doxie_config.png)

## Checking if the server is running

If you visit the address of the container in your browser you can check if the server is running.
A successfully configured server should return a message like 'Service is running.' and display the build number of the image.
