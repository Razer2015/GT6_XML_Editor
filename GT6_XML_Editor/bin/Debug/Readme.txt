*********************rail_def*********************
1. Death Valley - 6920060292a4d564e8855a046cd1c89a
2. Eifel - f705875b55bc4c844a97a097b4d2bedc
3. Andalusia - de936d380a916424f8c5db41b9707b4e 
5. Eifel Flat - e216b65de71a2574593620015a80c67a

********************coursemaker*******************
1. Death Valley - 765fb1a0d6b38b042a8c7153a1d0652e
2. Eifel - 0327ce0504420e54fa826624d0ed189c
3. Andalusia - d71517d7c4fbe714999594a8f346963e
5. Eifel Flat - cfe1e8bafd7bc0f45b09adfb04c84b82

*********************Tutorial*********************
1. Open the tool
2. File -> Open (rail_def)
3. Browse to your extracted unity asset or xml file (You might need to change it so you see "All files (*.*))
4. Xml file opens in the window so you can edit the entries
5. Modify the values you want by double clicking or selecting and clicking "Edit"
6. File -> Save or Save As
7. Enter the filename and the type (Plain XML file or Unity Asset), remember that you need Unity Asset if you want to place it inside the apk, plain xml is just if you want to modify it for example in Notepad++ instead of this tool
8. Once you have the modified unity asset, place it inside the APK with for example 7Zip
9. Now you need to resign in order to be able to install that modified APK
10. Click "Sign" and select the modified APK
11. New apk with *_signed.apk shows up and you are done

**********************Errors**********************
Make sure all files are where they should be

Make sure the file isn't open in some other location

There is two signing tools that this Tool uses, Java and C#. The Java one is a bit faster so it tries to use that first but if it doesn't succeed, it tries to use C# one. If you want to use Java, make sure you have Java installed and JAVA_HOME environment variable is pointing in the right location.

*********************Razerman*********************
*		Coded by Razerman		 *
*********************Razerman*********************