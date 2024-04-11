INCLUDE global.ink

{ knowPolarizer:
- false: ->intro
- true: ->repeat
}

=== intro ===
Hi, bro!
+ [I know you, you are a lens]
    I am not a lens, man. I am a polarizer.
+ [Who are you?]
    I am a polarizer, man. 
- In general, I could filter polarization light. #title:Polarizer #speaker:Polarizer
~ knowPolarizer = true
What do you want, man?
->choices

=== choices ===
+ [Tell me more about yourself, bro!]->polarizer
+ {isPolarizerTutFinish} [Let's do a quiz!]->quiz
+ [No other things I want to know.]->end

=== polarizer ===
I only allow the light that has polarization align with my polarization axis to pass through #title:Polarizer Knowledge
+[But I did not see polarization axis in UI]
    My polarization axis is originally allign with the x-axis. If you rotate me by change the rotation degree parameter, my polarization axis will change.
    
    ~ isPolarizerTutFinish = true
    ++ [CONTINUE]->repeat

=== quiz ===
My Polarization Axis is aligned with the x-axis, what kind of wave can NOT pass through me? #title:Polarizer Quiz #image:Tutorial/Polarizer/Polarization_Quiz_Question
+[Linear polarization align with x-axis]
    Incorrect! Since my axis is aligned with x-axis, I will let all the wave whose linear polarization align with x-axis pass through
    ++ [Redo the quiz] -> quiz
+[Linear polarization with 45 degree away from the x-axis]
    Incorrect! Wave with 45 degree linear polarization away from the x-axis still has wave component that is align with the y-axis, thus leaving this part of the wave passing through me #image:Tutorial/Polarizer/Polarization_Quiz_45_option
    ++ [Redo the quiz] -> quiz
+[Linear polarization align with y-axis]
    Correct!!! In this situation, wave's polarization is orthogonal to my axis, thus I will block everyting
    ++[CONTINUE]->repeat

=== repeat ===
What else do you want, bro? #title:Polarizer #speaker:Polarizer
->choices

=== end ===
You could invoke me again by clicking any polarizers. May you have a good learning experience!
+ [LEAVE]->DONE