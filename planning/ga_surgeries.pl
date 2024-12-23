:-dynamic generations/1.
:-dynamic population/1.
:-dynamic prob_crossover/1.
:-dynamic prob_mutation/1.
:- dynamic surgery_id/2.

:-dynamic max_time/1.
:-dynamic best_value_threshold/1.
:-dynamic stability_generations/1.
:- dynamic start_time/1.
:- dynamic population_history/2. % Dynamic predicate to store population history
:- dynamic surgery_penalty/3. % Dynamic predicate to store surgery penalty

% Agenda Staff
agenda_staff(d001,20241028,[(720,790,m01),(1080,1140,c01)]).
agenda_staff(d002,20241028,[(850,900,m02),(901,960,m02),(1380,1440,c02)]).
agenda_staff(d003,20241028,[(720,790,m01),(910,980,m02)]).
agenda_staff(d004,20241028,[(850,900,m02),(940,980,c04)]).

% Timetable
timetable(d001,20241028,(480,1200)).
timetable(d002,20241028,(500,1440)).
timetable(d003,20241028,(520,1320)).
timetable(d004,20241028,(620,1020)).

% Staff
staff(d001,doctor,orthopaedist,[so2,so3,so4]).
staff(d002,doctor,orthopaedist,[so2,so3,so4]).
staff(d003,doctor,orthopaedist,[so2,so3,so4]).
staff(d004,doctor,orthopaedist,[so2,so3,so4]).

surgery_id(so100001,so2).
surgery_id(so100002,so3).
surgery_id(so100003,so4).
%surgery_id(so100004,so2).
%surgery_id(so100005,so4).
%surgery_id(so100006,so2).
%surgery_id(so100007,so3).
%surgery_id(so100008,so2).
%surgery_id(so100009,so2).
%surgery_id(so100010,so2).
%surgery_id(so100011,so4).
%surgery_id(so100012,so2).
%surgery_id(so100013,so2)

% surgery(SurgeryType, TAnesthesia, TSurgery, TCleaning).
surgery(so2, 45, 60, 45).
surgery(so3, 45, 90, 45).
surgery(so4, 45, 75, 45).    
surgery(so5, 45, 60, 45).

assignment_surgery(so100001,d001).
assignment_surgery(so100002,d002).
assignment_surgery(so100003,d003).
%assignment_surgery(so100004,d001).
%assignment_surgery(so100004,d002).
%assignment_surgery(so100005,d002).
%assignment_surgery(so100005,d003).
%assignment_surgery(so100006,d001).
%assignment_surgery(so100007,d003).
%assignment_surgery(so100008,d004).
%assignment_surgery(so100008,d003).
%assignment_surgery(so100009,d002).
%assignment_surgery(so100009,d004).
%assignment_surgery(so100010,d003).
%assignment_surgery(so100011,d001).
%assignment_surgery(so100012,d001).
%assignment_surgery(so100013,d004).

% Agenda operation room
agenda_operation_room(or1,20241028,[(520,579,so100000),(1000,1059,so099999)]).

% New predicate for count surgeries
surgery_count(NumS) :-
    findall((X,Y), surgery_id(X, Y), SurgeryList),
    length(SurgeryList, NumS).

% parameters initialization
initialize:-write('Number of new generations: '),read(NG), 			
    (retract(generations(_));true), asserta(generations(NG)),
	write('Population size: '),read(PS),
	(retract(population(_));true), asserta(population(PS)),
	write('Probability of crossover (%):'), read(P1),
	PC is P1/100, 
	(retract(prob_crossover(_));true), 	asserta(prob_crossover(PC)),
	write('Probability of mutation (%):'), read(P2),
	PM is P2/100, 
	(retract(prob_mutation(_));true), asserta(prob_mutation(PM)),

    % Stop conditions
    write('Maximum time to run (in seconds): '), read(MaxTime),
    (retract(max_time(_));true), asserta(max_time(MaxTime)),
    write('Best value threshold to stop: '), read(BestValueThreshold),
    (retract(best_value_threshold(_));true), asserta(best_value_threshold(BestValueThreshold)),
    write('Number of generations for population stability: '), read(StabilityGenerations),
    (retract(stability_generations(_));true), asserta(stability_generations(StabilityGenerations)),

    %Elitism
    write('Selection method (1 for purely elitist, 2 for no purely elitist): '), read(SM),
    (retract(selection_method(_));true), asserta(selection_method(SM)).

generate:-
    initialize,
    start_timer,  % Start the timer at the beginning of the generation process
    generate_population(Pop),
    write('Pop='),write(Pop),nl,
    evaluate_population(Pop,PopValue),
    write('PopValue='),write(PopValue),nl,
    order_population(PopValue,PopOrd),
    generations(NG),
    max_time(MaxTime),
    best_value_threshold(BestValueThreshold),
    stability_generations(StabilityGenerations),
    generate_generation(0,NG,MaxTime,BestValueThreshold,StabilityGenerations,PopOrd).

generate_population(Pop):-
    population(PopSize),
    surgery_count(NumT), % Count the number of surgeries
    write('Counter:'), write(NumT),
    forall(surgery_id(Surgery, Type),
    assertz(surgery_penalty(Surgery, Type, 0))),
    findall(Surgery,surgery_penalty(Surgery,_,_),SurgeriesList), % Changed to surgeries
    generate_population(PopSize,SurgeriesList,NumT,Pop).

generate_population(0,_,_,[]):-!.
generate_population(PopSize,SurgeriesList,NumT,[Ind|Rest]):-
    PopSize1 is PopSize-1,
    generate_population(PopSize1,SurgeriesList,NumT,Rest),
    generate_individual(SurgeriesList,NumT,Ind),
    not(member(Ind,Rest)).
generate_population(PopSize,SurgeriesList,NumT,L):-
    generate_population(PopSize,SurgeriesList,NumT,L).


generate_individual([G],1,[G]):-!.

generate_individual(SurgeriesList,NumT,[G|Rest]):-
    NumTemp is NumT + 1, % to use with random
    random(1,NumTemp,N),
    remove(N,SurgeriesList,G,NewList),
    NumT1 is NumT-1,
    generate_individual(NewList,NumT1,Rest).

remove(1,[G|Rest],G,Rest).
remove(N,[G1|Rest],G,[G1|Rest1]):- N1 is N-1,
            remove(N1,Rest,G,Rest1).


evaluate_population([],[]).
evaluate_population([Ind|Rest],[Ind*V|Rest1]):-
    evaluate(Ind,V),
    evaluate_population(Rest,Rest1).

evaluate(Seq,V):- evaluate(Seq,0,V).

evaluate([ ],_,0).
evaluate([Surgery|Rest], TotalTime, V) :-
    best_value_threshold(BestValueThreshold),
   
    surgery_penalty(Surgery, SurgeryType, Penalty),
    surgery_time(SurgeryType, SurgeryTime),
   
    NewTotalTime is TotalTime + SurgeryTime,
   
    % Check if the total time is within the valid value
    ( NewTotalTime =< BestValueThreshold ->

      evaluate(Rest, NewTotalTime, VRest),
      V is NewTotalTime + VRest + Penalty
    ; 
      % Outside the limit, increment the penalty with the exceeded time
      ExceedPenalty is Penalty + (NewTotalTime - BestValueThreshold),
      evaluate(Rest, NewTotalTime, VRest),
      V is NewTotalTime + VRest + ExceedPenalty
    ).
   
   
% Calculate the total time of a surgery
surgery_time(SurgeryType, TotalTime) :-
    surgery(SurgeryType, Time1, Time2, Time3),
    TotalTime is Time1 + Time2 + Time3.

order_population(PopValue,PopValueOrd):-
    bsort(PopValue,PopValueOrd).

bsort([X],[X]):-!.
bsort([X|Xs],Ys):-
    bsort(Xs,Zs),
    bchange([X|Zs],Ys).


bchange([X],[X]):-!.

bchange([X*VX,Y*VY|L1],[Y*VY|L2]):-
    VX>VY,!,
    bchange([X*VX|L1],L2).

bchange([X|L1],[X|L2]):-bchange(L1,L2).
    
generate_generation(G, G, _, _, _, Pop) :- !,
    write('Final Generation '), write(G), write(':'), nl, 
    write(Pop), nl.

generate_generation(N, MaxGenerations, MaxTime, BestValueThreshold, StabilityGenerations, Pop) :-
    get_time(CurrentTime),
    start_time(StartTime),
    ElapsedTime is CurrentTime - StartTime,
    
    (ElapsedTime > MaxTime ->
        write('Maximum time reached! Stopping at generation '), write(N), nl
    ;
        (
            write('Generation '), write(N), write(':'), nl, write(Pop), nl,
            Pop = [Best*BestValue|_],

            (check_population_stability(N, Pop, StabilityGenerations) ->
                write('Population stabilized after '), write(StabilityGenerations),
                write(' generations. Stopping.'), nl
            ;
                (BestValue =< BestValueThreshold ->
                    write('Stopping condition met! Best value: '), write(BestValue), nl,
                    write('Stopping at generation '), write(N), nl
                ;
                    crossover(Pop, NPop1),
                    mutation(NPop1, NPop),
                    evaluate_population(NPop, NPopValue),
                    order_population(NPopValue, NPopOrd),
                    
                    write('Best Value: '), write(BestValue), nl,

                    selection_method(Method),
                    (Method = 1 ->
                        % Purely elitist selection
                        (NPopOrd = [_NextBest*NextBestValue|_],
                        write('Next Best Value: '), write(NextBestValue), nl,
                        (NextBestValue @=< BestValue ->
                            FinalPop = NPopOrd
                        ;
                            add_best(Best*BestValue, NPopOrd, FinalPop)
                        ))
                    ;
                        random(0.0, 1.0, R),
                        (R < 0.6 ->
                            % 60% chance: Tournament selection normal
                            select_population(NPopOrd, SelectedPop),
                            add_best(Best*BestValue, SelectedPop, FinalPop)
                        ;
                            % 40% chance: Seleção baseada em rank
                            NPopOrd = [Current*CurrentValue|_],
                            write('Current Value: '), write(CurrentValue), nl,
                            CompareValue is BestValue * 1.2,
                            write('CompareValue: '), write(CompareValue), nl,
                            (CurrentValue @< BestValue * 1.2 ->
                                % Se o melhor atual estiver próximo do melhor global (até 20% pior)
                                % mantém a população atual
                                FinalPop = NPopOrd
                            ;
                                % Caso contrário, adiciona o melhor global
                                add_best(Best*BestValue, NPopOrd, FinalPop)
                            )
                        )
                    ),
                    
                    N1 is N + 1,
                    store_population(N, FinalPop),
                    generate_generation(N1, MaxGenerations, MaxTime, BestValueThreshold, StabilityGenerations, FinalPop)
                )
            )
        )
    ).
        

% Add the best in the first position
add_best(Best, Pop, FinalPop) :-
    append(Front, [_|Rest], Pop),
    append(Front, Rest, TempPop),
    append([Best], TempPop, FinalPop).

start_timer :-
    get_time(CurrentTime),
    retractall(start_time(_)),
    asserta(start_time(CurrentTime)).

% Store a population for a given generation
store_population(Generation, Population) :-
    % Retract any previous history for this generation
    retract(population_history(Generation, _)),
    fail.
store_population(Generation, Population) :-
    assertz(population_history(Generation, Population)).

% Check population stability
check_population_stability(CurrentGeneration, CurrentPop, StabilityGenerations) :-
    CurrentGeneration >= StabilityGenerations,
    check_last_n_generations(CurrentGeneration, StabilityGenerations, CurrentPop).

% Check if the last N generations have the same population
check_last_n_generations(_, 0, _) :- !.
check_last_n_generations(CurrentGeneration, RemainingGenerations, CurrentPop) :-
    PreviousGeneration is CurrentGeneration - 1,
    population_history(PreviousGeneration, PreviousPop),
    populations_are_identical(CurrentPop, PreviousPop),
    
    NextRemainingGenerations is RemainingGenerations - 1,
    check_last_n_generations(PreviousGeneration, NextRemainingGenerations, CurrentPop).

% Check if two populations are identical
populations_are_identical(Pop1, Pop2) :-
    length(Pop1, Len),
    length(Pop2, Len),
    compare_population_individuals(Pop1, Pop2).

% Compare each individual in the populations
compare_population_individuals([], []).
compare_population_individuals([Individual1*Value1|Rest1], [Individual2*Value2|Rest2]) :-
    Individual1 = Individual2,
    Value1 = Value2,
    compare_population_individuals(Rest1, Rest2).

% Tournament selection
tournament_select(Pop, Selected) :-
    population(PopSize),
    random(0, PopSize, Idx1),
    random(0, PopSize, Idx2),
    nth0(Idx1, Pop, Ind1*Val1),
    nth0(Idx2, Pop, Ind2*Val2),
    (Val1 =< Val2 -> Selected = Ind1*Val1 ; Selected = Ind2*Val2).

% Modified selection process
select_population(OldPop, NewPop) :-
    population(PopSize),
    select_population(OldPop, PopSize, [], NewPop).

select_population(_, 0, Acc, Acc) :- !.
select_population(OldPop, N, Acc, NewPop) :-
    tournament_select(OldPop, Selected),
    N1 is N - 1,
    select_population(OldPop, N1, [Selected|Acc], NewPop).

generate_crossover_points(P1,P2):- generate_crossover_points1(P1,P2).
generate_crossover_points1(P1,P2):-
    surgery_count(N), % number of surgeries
    NTemp is N+1,
    random(1,NTemp,P11),
    random(1,NTemp,P21),
    P11\==P21,!,
    ((P11<P21,!,P1=P11,P2=P21);P1=P21,P2=P11).
    generate_crossover_points1(P1,P2):-
    generate_crossover_points1(P1,P2).


crossover([ ],[ ]).
crossover([Ind*_],[Ind]).
crossover([Ind1*_,Ind2*_|Rest],[NInd1,NInd2|Rest1]):-

    % Aleatoriedade na seleção de quais indivíduos fazer crossover
    random(0, 2, Rand),

    % Seleciona dois indivíduos aleatórios na lista
    length(Rest, Len),
    (Len > 0 -> 
        random(0, Len, RandomIndex),
        
        nth0(RandomIndex, Rest, NextInd*_) % Pega o indivíduo no índice aleatório
    ;  
        NextInd = Ind2 % Caso não exista mais indivíduos, fica o atual
    ),
    
    % Realiza crossover com o indivíduo selecionado aleatoriamente
    (Rand =:= 0 -> 
        generate_crossover_points(P1,P2),
        prob_crossover(Pcruz),
        random(0.0,1.0,Pc),
        ((Pc =< Pcruz,!,
        cross(Ind1,NextInd,P1,P2,NInd1),
        cross(NextInd,Ind1,P1,P2,NInd2))
        ;
        (NInd1=Ind1,NInd2=NextInd))
    ;
        % Se Rand não for 0, avançamos esses indivíduos
        NInd1=Ind1, NInd2=NextInd
    ),
    crossover(Rest,Rest1).


fillh([ ],[ ]).

fillh([_|R1],[h|R2]):-
	fillh(R1,R2).

sublist(L1,I1,I2,L):-I1 < I2,!,
    sublist1(L1,I1,I2,L).

sublist(L1,I1,I2,L):-sublist1(L1,I2,I1,L).

sublist1([X|R1],1,1,[X|H]):-!, fillh(R1,H).

sublist1([X|R1],1,N2,[X|R2]):-!,N3 is N2 - 1,
	sublist1(R1,1,N3,R2).

sublist1([_|R1],N1,N2,[h|R2]):-N3 is N1 - 1,
		N4 is N2 - 1,
		sublist1(R1,N3,N4,R2).

rotate_right(L,K,L1):- surgery_count(N), % number of surgeries
	T is N - K,
	rr(T,L,L1).

rr(0,L,L):-!.

rr(N,[X|R],R2):- N1 is N - 1,
	append(R,[X],R1),
	rr(N1,R1,R2).

remove([],_,[]):-!.

remove([X|R1],L,[X|R2]):- not(member(X,L)),!,
        remove(R1,L,R2).

remove([_|R1],L,R2):-
    remove(R1,L,R2).

insert([],L,_,L):-!.
insert([X|R],L,N,L2):-
    surgery_count(T), % number of surgeries
    ((N>T,!,N1 is N mod T);N1 = N),
    insert1(X,N1,L,L1),
    N2 is N + 1,
    insert(R,L1,N2,L2).


insert1(X,1,L,[X|L]):-!.
insert1(X,N,[Y|L],[Y|L1]):-
    N1 is N-1,
    insert1(X,N1,L,L1).

cross(Ind1,Ind2,P1,P2,NInd11):-
    sublist(Ind1,P1,P2,Sub1),
    surgery_count(NumT), % number of surgeries
    R is NumT-P2,
    rotate_right(Ind2,R,Ind21),
    remove(Ind21,Sub1,Sub2),
    P3 is P2 + 1,
    insert(Sub2,Sub1,P3,NInd1),
    removeh(NInd1,NInd11).


removeh([],[]).

removeh([h|R1],R2):-!,
    removeh(R1,R2).

removeh([X|R1],[X|R2]):-
    removeh(R1,R2).

mutation([],[]).
mutation([Ind|Rest],[NInd|Rest1]):-
	prob_mutation(Pmut),
	random(0.0,1.0,Pm),
	((Pm < Pmut,!,mutacao1(Ind,NInd));NInd = Ind),
	mutation(Rest,Rest1).

mutacao1(Ind,NInd):-
	generate_crossover_points(P1,P2),
	mutacao22(Ind,P1,P2,NInd).

mutacao22([G1|Ind],1,P2,[G2|NInd]):-
	!, P21 is P2-1,
	mutacao23(G1,P21,Ind,G2,NInd).
mutacao22([G|Ind],P1,P2,[G|NInd]):-
	P11 is P1-1, P21 is P2-1,
	mutacao22(Ind,P11,P21,NInd).

mutacao23(G1,1,[G2|Ind],G2,[G1|Ind]):-!.
mutacao23(G1,P,[G|Ind],G2,[G|NInd]):-
	P1 is P-1,
	mutacao23(G1,P1,Ind,G2,NInd).