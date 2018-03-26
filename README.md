# EZActions
EZActions is a fast, simple and lightweight Visual Studio Extension for ASP.NET MVC that can generate a JavaScript file based on jQuery to provide a direct way to access and call your MVC Actions using custom data annotations without the hassle of hardcoding custom Ajax calls.

# Introduction
Are you tired of constantly writing, updating and handling custom Ajax calls from all of your MVC Controller Actions? Look no further! 

EZActions was made to make peoples life using MVC Actions in a JavaScript environment easier than ever before! With this extension, it's as simple as a click of a button to generate a JavaScript file which takes care of everything for you.

**Download (v1.0):** [https://marketplace.visualstudio.com/items?itemName=kevinjp.EZActions](https://marketplace.visualstudio.com/items?itemName=kevinjp.EZActions)

# Quick start / Setup
After you've successfully installed EZActions from the above link, there's an Option you need to setup to tell EZActions where it should generate your newly created EZActions.js file. By default there's no path so you need to set this yourself in Options.

1. Go to **'Tools -> Options -> Scroll down on the list till you see EZActions -> General -> Generated JavaScript File Path'**, and fill out this field with the path to the directory you want to save the file in.

2. After you've filled it out, click **OK**.

3. Now you are ready to generate your JavaScript file! Go to **'Tools'** and hit the **'Generate EZActions'** button with a green EZ- icon. 

4. If everything wen't well, it will popup with a message saying it was successfully generated in the folder that you specified earlier.

5. You are now good to go! Insert a reference to the JavaScript file in your Layout/or any other file and simply access the object **'EZActions'** in JavaScript which will contain all of your Controllers and their Actions.

# Open source
If you want to, you can follow the development or contribute on this Github! I would love to answer questions or any issues aswell!

# Credits
Made by Kevin Jensen Petersen (LinkedIn: [https://www.linkedin.com/in/publicvoid/](https://www.linkedin.com/in/publicvoid/))
