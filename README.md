<h1>Gran Turismo 6 Unity Asset XML Editor</h1>

<h4>rail_def -files (v1.0.0)</h4>
1. Death Valley - 6920060292a4d564e8855a046cd1c89a<br>
2. Eifel - f705875b55bc4c844a97a097b4d2bedc<br>
3. Andalusia - de936d380a916424f8c5db41b9707b4e <br>
5. Eifel Flat - e216b65de71a2574593620015a80c67a<br>
<br>

<h4>coursemaker -files (v1.0.0)</h4>
1. Death Valley - 765fb1a0d6b38b042a8c7153a1d0652e<br>
2. Eifel - 0327ce0504420e54fa826624d0ed189c<br>
3. Andalusia - d71517d7c4fbe714999594a8f346963e<br>
5. Eifel Flat - cfe1e8bafd7bc0f45b09adfb04c84b82<br>
<br>

<h2>Tutorial</h2>
1. Open the tool<br>
2. File -> Open (rail_def)<br>
3. Browse to your extracted unity asset or xml file (You might need to change it so you see "All files (*.*))<br>
4. Xml file opens in the window so you can edit the entries<br>
5. Modify the values you want by double clicking or selecting and clicking "Edit"<br>
6. File -> Save or Save As<br>
7. Enter the filename and the type (Plain XML file or Unity Asset), remember that you need Unity Asset if you want to place it inside the apk, plain xml is just if you want to modify it for example in Notepad++ instead of this tool<br>
8. Once you have the modified unity asset, place it inside the APK with for example 7Zip<br>
9. Now you need to resign in order to be able to install that modified APK<br>
10. Click "Sign" and select the modified APK<br>
11. New apk with *_signed.apk shows up and you are done<br>
<br>

<h2>Errors</h2>
Make sure all files are where they should be<br>
<br>
Make sure the file isn't open in some other location<br>
<br>
There is two signing tools that this Tool uses, Java and C#. The Java one is a bit faster so it tries to use that first but if it doesn't succeed, it tries to use C# one. If you want to use Java, make sure you have Java installed and JAVA_HOME environment variable is pointing in the right location.<br>
<br>

<h3>Coded by Razerman</h3>
