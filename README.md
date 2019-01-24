# Extract File Properties
ExtractFileProperties is a command line utility that reads the properties from a Windows file and writes them out as XML. It is written in C#, requiring .Net Framework 4.5.2.

The command line looks like this:

ExtractFileProperties [-a] [-v] < one or more filenames >

The output is an XML file containing the properties, written to the same directory as the source file, under the name of the source file with .props.xml appended to it. So, example, if the source file is video.mp4, XML properties file will be video.mp4.props.xml.

By default, only the System.Keywords (known to the rest of us as tags) and System.Comment properties are extracted.

If the -a option is specified, the values of all the available properties are extracted.

If the -v option is specified, a line of output is written for every file that is processed.

## Installation

To install a binary:
1.	Choose a release, then download the ExtractFileProperties.zip file attached to it.
2.	Extract the contents of the zip file to a programs folder.
3.	Run ExtractFileProperties.exe with appropriate parameters.

## Release History

* 1.0
    * The first release

## Meta

Distributed under the MS-PL license. See [license](license.md) for more information.



