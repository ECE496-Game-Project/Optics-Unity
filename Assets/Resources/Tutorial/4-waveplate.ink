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
+ [I know you, you are a Polarizer.]
    Bro, that is my brother. I am the Waveplate.
+ [Wait, have we met before?]
    No, not at all. You must have met my brother. My name is Waveplate. 
- In general, I could rotate polarization light. #title:Waveplate #speaker:Waveplate
~ knowWaveplate = true
Why do you come here?
->choices

=== choices ===
+ [Could you tell me more about yourself?]->waveplate
+ {isWaveplateTutFinish} [Let's do a quiz!]->quiz
+ [No other things I want to know.]->end

=== waveplate ===
Depend on the thickness of mine, I will increase the phase difference(Retardation) between the slow and fast axis. If the Retardation is 90 degree, i'm a quarter-waveplate. If the Retardation is 180 degree, i'm a half-waveplate.
Try to rotate me and see some cool effects! #title:Waveplate Knowledge
~ isWaveplateTutFinish = true
->choices
+ [CONTINUE]->repeat

=== quiz ===
If the light shooting into me is linearly polarized, and I'm a quarter-waveplate, what will the output light be? #title:Waveplate Quiz
    + [Circular polarization]
        Your answer is correct
        ++ [CONTINUE]-> repeat
    + [Linear polarization]
        Your answer is incorrect->quiz

=== repeat ===
Do you have any other things to ask? #title:Waveplate #speaker:Waveplate
->choices

=== end ===
You could invoke me again by clicking any waveplates. May you have a good learning experience!
+ [LEAVE]->DONE