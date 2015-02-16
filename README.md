# MazBlog

Este repositorio contiene un proyecto que muestra el uso de ASP.NET Web API, [AtomPub](https://tools.ietf.org/html/rfc5023) y Azure Storage. Para hacer publicación de contenido web haciendo uso de este estándar y clientes como Windows Live Writer

## Acerca del proyecto

* Cómo mucho de lo que por aquí publico… esto es por propósitos de aprendizaje.
* Este proyecto no ha sido puesto en producción aún

## Correr el servidor

El proyecto es Web Hosting y será necesario realizar un par de pasos previos antes de poderlo correr. El proyecto tiene la url (http://dev.mazblog.net/), esto por poder usar el Fiddler sin ningún problema sobre el localhost. Para poder usar este alias habrá que crear el respectivo alias en el host.config.
El Azure Storage Emulator debe estar corriendo para poder usar el servidor.

## Usar desde Windows Live Writer

**Nota**: El usuario y contraseña están almacenados (un string plano) en el WeConfig. En desarrollo he dejado:

* Usuario: user 
* Contraseña: password

Todas las pruebas han sido realizadas haciendo uso del Windows Live Writer ([¡Si, no muere!](https://twitter.com/shanselman/status/563083750666670080)) Para agregarlo:

* Seleccionar Atom Publishing Protocol, como el tipo de blog a usar y la url del documento del servicio será: http://dev.mazblog.net/api/discovery

## Acerca del código (Créditos)

* Mucho del código aquí empleado ha sido tomado de la [serie de artículos](http://benfoster.io/blog/atompub-aspnet-web-api-part1) publicados por Ben Foster. Yo solo agregué autenticación y [soporte para imágenes](https://nicolocodev.wordpress.com/2013/06/06/web-api-atompub-y-web-api-soporte-para-multimedia/)

* Las “capacidades” del blog han sido definidas haciendo uso del archivo de manifiesto y empleando algunos de los  elementos [aquí detallados](https://msdn.microsoft.com/en-us/library/bb463260.aspx)

## TO DO:

* Terminar los TODO que hay en el código :P

* Terminar Tests de integración

* Crear un .ps para la publicación del blog en un Azure Web Site
