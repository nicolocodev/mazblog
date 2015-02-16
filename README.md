# MazBlog

Este repositorio contiene un proyecto que muestra el uso de ASP.NET Web API, [AtomPub](https://tools.ietf.org/html/rfc5023) y Azure Storage. Para hacer publicaci�n de contenido web haciendo uso de este est�ndar y clientes como Windows Live Writer

## Acerca del proyecto

* C�mo mucho de lo que por aqu� publico� esto es por prop�sitos de aprendizaje.
* Este proyecto no ha sido puesto en producci�n a�n

## Correr el servidor

El proyecto es Web Hosting y ser� necesario realizar un par de pasos previos antes de poderlo correr. El proyecto tiene la url (http://dev.mazblog.net/), esto por poder usar el Fiddler sin ning�n problema sobre el localhost. Para poder usar este alias habr� que crear el respectivo alias en el host.config.
El Azure Storage Emulator debe estar corriendo para poder usar el servidor.

## Usar desde Windows Live Writer

**Nota**: El usuario y contrase�a est�n almacenados (un string plano) en el WeConfig. En desarrollo he dejado:

* Usuario: user 
* Contrase�a: password

Todas las pruebas han sido realizadas haciendo uso del Windows Live Writer ([�Si, no muere!](https://twitter.com/shanselman/status/563083750666670080)) Para agregarlo:

* Seleccionar Atom Publishing Protocol, como el tipo de blog a usar y la url del documento del servicio ser�: http://dev.mazblog.net/api/discovery

## Acerca del c�digo (Cr�ditos)

* Mucho del c�digo aqu� empleado ha sido tomado de la [serie de art�culos](http://benfoster.io/blog/atompub-aspnet-web-api-part1) publicados por Ben Foster. Yo solo agregu� autenticaci�n y [soporte para im�genes](https://nicolocodev.wordpress.com/2013/06/06/web-api-atompub-y-web-api-soporte-para-multimedia/)

* Las �capacidades� del blog han sido definidas haciendo uso del archivo de manifiesto y empleando algunos de los  elementos [aqu� detallados](https://msdn.microsoft.com/en-us/library/bb463260.aspx)

## TO DO:

* Terminar los TODO que hay en el c�digo :P

* Terminar Tests de integraci�n

* Crear un .ps para la publicaci�n del blog en un Azure Web Site
