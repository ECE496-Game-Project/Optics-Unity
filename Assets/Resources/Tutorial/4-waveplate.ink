INCLUDE global.ink

{ knowWaveplate:
- false: ->intro1
- true: ->repeat
}

=== intro1 ===
... ...
+ [Hello?] ->intro2
+ [... ...] ->intro1

=== intro2 ===
Oh, hello. Sorry, I didn't see you.
+ [I know you, you are a polarizer]
    Bro, that is my brother. I am the waveplate.
+ [Wait, have we met before?]
    No, not at all. You must have met my brother. My name is waveplate. 
- In general, wave that has different polarization will behave differently when passing through me . #title:Waveplate #speaker:Waveplate
~ knowWaveplate = true
Why do you come here?
->choices

=== choices ===
+ [Could you tell me more about yourself?]->waveplate
+ {isWaveplateTutFinish} [Let's do a quiz!]->quiz
+ [No other things I want to know.]->end

=== waveplate ===
I am a wave plate. Just like my brother, the polarizer, who has a polarization axis, I have not just one, but two axes!! I call them the slow axis and the fast axis. #title:Waveplate Knowledge #image:Tutorial/Waveplate/Waveplate_axis
    +[Why are they so speical?]
        I am glad you ask! Simply speaking, light wave that has polarization align with my fast axis and slow axis will behave differently.
        For the light that has polarization aligned with my slow axis is travelling faster comparing to the light that has polarization aligned with my fast axis.
        Because of this, the wave that passes through me will create a phase difference between the two components of the wave.
        ++[How much phase difference is there?]
        Good Question, you can customize how much phase differece by adjusting the retardation parameter. There are special name for two phase difference.
        When it is 90 degree, you can call me quarter-waveplate
        When it is 180 degree, you can call me half-waveplate
        +++[Can I rotate you?]
            By default, my slow axis is aligned with the x-axis and fast-axis is aligned with the y-axis. You can rotating my axis by changing the rotation degree parameter. For example when it is 45 degree, my axis will look like this. #image: Tutorial/Waveplate/waveplate_45_rotation
            ~ isWaveplateTutFinish = true
            ++++ [CONTINUE]->repeat

=== quiz ===
What polarization of the wave will become spherical polarization after passing through a quater wave plate #title:Waveplate Quiz
    +[Linear Polarization]
    Correct!!!
        ++ [CONTINUE]->repeat
    +[Spherical Polarization]
    You almost got it right. When Spherical polarization passes through,
        ++[Redo the Quiz]->quiz

=== repeat ===
Do you have any other things to ask? #title:Waveplate #speaker:Waveplate
->choices

=== end ===
You could invoke me again by clicking any waveplates. May you have a good learning experience!
+ [LEAVE]->DONE