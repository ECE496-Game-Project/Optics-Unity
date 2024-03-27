#title:Wave Source #speaker:WAVE SOURCE
-> quiz

=== quiz ===
VAR answer = 0
What's the answer?
    + [1]
        ~ answer = 1
    + [2]
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
