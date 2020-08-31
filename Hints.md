# Hints to assignment

Talk together cross teams, and help each other.

If you are stuck with the code, put some time on the articles in DocsSrc\Articles, remeber that it´s possible to run the documentation as a websever a access it using a browser.

* Google for similar projects: HttpListner simple webserver
* Make the HttpListener continiue to run, so that more requests can be made to the server
* Based on the URL load a matching file from Content-folder, hint: See the property RawUrl on the request-object
* Set the repsonse parameters, status and content-type
* Implement the dynamic page
* Act on the response type
* Implement the cookie page

## Introduction to the internet
Video: [History of the Internet](https://www.youtube.com/watch?v=9hIQjrMHTv4)

Video: [How the Internet Works in 5 Minutes](https://www.youtube.com/watch?v=7_LPdttKXPc)

Video: [URIs, URLs, and URNs](https://www.youtube.com/watch?v=vpYct2npKD8)

RFC: [Uniform Resource Identifier (URI): Generic Syntax](https://tools.ietf.org/html/rfc3986)  for the hardcore!

Video: [How Do Web Browsers Work?](https://www.youtube.com/watch?v=WjDrMKZWCt0) by Dave Xiang

## Introduction to git
Video: [Git and GitHub for Beginners](https://www.youtube.com/watch?v=GqNAD4XoZ6k)

The standard for writing text on github is Markdown, a very simple syntax without many possibilities. [Mastering markdown](https://guides.github.com/features/mastering-markdown/)

## Introduktion to HTTP
Article: [HTTP: The Protocol Every Web Developer Must Know](https://code.tutsplus.com/tutorials/http-the-protocol-every-web-developer-must-know-part-1--net-31177)

Video: [What is HTTP?](https://www.youtube.com/watch?v=SzSXHv8RKdM) by Dave Xiang

Video: [The Http and the Web](https://www.youtube.com/watch?v=eesqK59rhGA) by WebDev Cave

RFC: [Hypertext Transfer Protocol -- HTTP/1.1](https://tools.ietf.org/html/rfc2616) for the hardcore!

### HTTP headers used in the project

* Request: [accept](https://en.wikipedia.org/wiki/Content_negotiation)
* Request: [Request method](https://en.wikipedia.org/wiki/Hypertext_Transfer_Protocol#Request_methods)
* Request/Response: [Content type](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Type) [MIME types](https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Complete_list_of_MIME_types)
* Request/Response: [Cookie](https://en.wikipedia.org/wiki/HTTP_cookie)
* Request: [Query string parameters](https://en.wikipedia.org/wiki/Query_string)
* Reponse: [Etag](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/ETag)
* Response: [Expires](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Expires)
* Response: [status codes](https://en.wikipedia.org/wiki/List_of_HTTP_status_codes)

## Test

In the respository is there a solution called IntegrationTests. This contains two projects, a unittest project and a console application project.

If you run the IntegrationTest-console application while having your webserver running will it run a collection of tests against your webserver, these tests matches the requirements in the assignment.

## Other 

A bit on [MD5 hashing](https://www.makeuseof.com/tag/md5-hash-stuff-means-technology-explained/)

## A bit of comedy
[The Internet](https://www.youtube.com/watch?v=iDbyYGrswtg) followed by the [speach about the Internet](https://www.youtube.com/watch?v=Vywf48Dhyns)
