INCLUDE global.ink

#title:System #speaker:Moon #portrait:Moon
Hello, I am Moon, the virtual agent of the OPTICS platform.
You could ask me anything you want to know.
-> choices

=== choices ===
+ [How to control the OPTICS scene?] -> control
+ [Who are you?] -> personal
+ [No other things I want to know] -> end

=== personal ===
I'm glad you ask! #title:Personal Information
My name is Moon, my avatar comes from the classical album "The Dark Side of the Moon". It is an album made by the band Pink Floyd in 1973. #image:Icons/logo
I use this album cover as my avatar because it is the classical dispersion experiment made by Newton using prism. 
I hope this platform could act like the prism to the light. Make you truly understand knowledge inside the OPTICS.
* [CONTINUE]->repeat

=== control ===
Welcome to the control tutorial! #title:Control
Are familiar with the control of 3D software? 
    * [Yes, I am an expert.]
        Since you are familiar with them, I would just list all the controls. Feel free to try it out!
        ** [CONTINUE]
            Camera Control:
            Click [A], [D] to move camera horizontally.
            Click [Q], [E] to move camera vertically.
            Click [W], [S] to zoom in and zoom out the camera.
            Slide Mouse wheel to zoom in and zoom out the camera.
            At Windows: Click [ALT] + Right Mouse to drag camera around. 
            Click [ALT] + Left Mouse to rotate camera around.
            At Mac: Click [OPTION] + Right Mouse to drag camera around.
            Click [OPTION] + Left Mouse to rotate camera around.
            Interaction Control:
            Click Left Mouse at any object in the scene to see details.
            Click Input Fields to input parameters.
            Drag Sliders to modify parameter smoothly.
    * [No , I am a beginner]
        I am going to introduce the basic control methods of the scene to you one by one.
        ** [CONTINUE]
            Do you prefer to use keyboard or mouse to control camera?
            *** [Keyboard]
                Click [A], [D] to move camera horizontally.
                Click [Q], [E] to move camera vertically.
                Click [W], [S] to zoom in and zoom out the camera.
                **** [CONTINUE]
            *** [Mouse]
                Are you using Mac keyboard?
                **** [Yes]
                    Slide Mouse wheel to zoom in and zoom out the camera.
                    Click [OPTION] + Right Mouse to drag camera around.
                    Click [OPTION] + Left Mouse to rotate camera around.
                    ***** [CONTINUE]
                **** [No]
                    Slide Mouse wheel to zoom in and zoom out the camera.
                    Click [ALT] + Right Mouse to drag camera around.
                    Click [ALT] + Left Mouse to rotate camera around.
                    ***** [CONTINUE]
        -- Click Left Mouse at any object in the scene to see details.
        Click Input Fields to input parameters.
        Drag Sliders to modify parameter smoothly.
-   * [CONTINUE]->repeat

=== repeat ===
Do you have any other things want to know? #title:System 
->choices

=== end ===
You could invoke me again by clicking the button with logo at anytime. May you have a good learning experience!
* [LEAVE]->DONE