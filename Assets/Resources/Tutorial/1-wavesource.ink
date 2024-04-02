-> wavesource

=== wavesource ===
Hello, there!
I know you are wondering what I am.
My name is Wave Source. #title:Wave Source #speaker:WAVE SOURCE #portrait:WAVE_SOURCE
I produce light.
Let's do a quiz!
->quiz
* [LEAVE]
->END


=== quiz ===
VAR answer = 0
What's the answer?
    + [Answer is 1]
        ~ answer = 1
    + [Answer is 2]
        ~ answer = 2
- 
{ answer:
- 1: 
    Your answer is {answer}, it is correct
    -> DONE
- else: 
    Your answer is {answer}, it is incorrect
    ->quiz
}
-> DONE
