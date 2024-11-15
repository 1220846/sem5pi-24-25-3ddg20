% Bibliotecas HTTP
:- use_module(library(http/thread_httpd)).
:- use_module(library(http/http_dispatch)).
:- use_module(library(http/http_parameters)).
:- use_module(library(http/http_client)).
:- use_module(library(http/http_open)).

% Bibliotecas JSON
:- use_module(library(http/json_convert)).
:- use_module(library(http/http_json)).
:- use_module(library(http/json)).

backend_url('https://localhost:5001/api/').

http:location(api, '/server', []).

% Criação de servidor HTTP em 'Port' que trata pedidos em JSON
server(Port) :-
    http_server(http_dispatch, [port(Port)]).

%-------------------------OperationTypes - SurgerysIds----------------------------%
:- http_handler(api(surgeryids), get_surgery_ids, []).

% GET SurgeryIds - OperationTypes
get_surgery_ids(_Request):-
    get_operation_types_backend(OperationTypes),
    maplist(convert_operation_type_to_surgery_id, OperationTypes, SurgeryIds),
    maplist(convert_surgery_id_to_json, SurgeryIds, JSON_SurgeryIds),
    reply_json(json([surgery_ids=JSON_SurgeryIds]), [json_object(dict)]).

% GET OperationTypes Backend
get_operation_types_backend(OperationTypes) :-
    backend_url(URL),
    atom_concat(URL, 'operationtypes', URL_OperationTypes),
    setup_call_cleanup(
        http_open(URL_OperationTypes, In, [cert_verify_hook(cert_accept_any)]),
        (read_string(In, _, Response),
         atom_json_dict(Response, OperationTypes, [])),
        close(In)).

% Convert OperationType dictionary to surgery_id
convert_operation_type_to_surgery_id(OperationTypeDict, surgery_id(Id, Name)) :-
    Id = OperationTypeDict.get(id),
    Name = OperationTypeDict.get(name).

% Convert surgery_id to JSON
convert_surgery_id_to_json(surgery_id(Id, Name), json([id = Id, name = Name])).

% Alias for surgery_id
surgery_id(Id, Name) :- convert_operation_type_to_surgery_id(_, surgery_id(Id, Name)).

%-------------------------OperationRequest - AssigmentSurgery--------------------------%
:- http_handler(api(assignmentsurgeries), get_assignment_surgeries, []).

% GET AssignmentSurgeries - OperationRequests
get_assignment_surgeries(_Request) :-
    get_operation_requests_backend(OperationRequests),
    maplist(convert_operation_request_to_surgery_id, OperationRequests, AssignmentSurgeries),
    maplist(convert_assignment_surgery_to_json, AssignmentSurgeries, JSON_AssignmentSurgeries),
    reply_json(json([assignment_surgeries = JSON_AssignmentSurgeries]), [json_object(dict)]).

% GET OperationRequests Backend
get_operation_requests_backend(OperationRequests) :-
    backend_url(URL),
    atom_concat(URL, 'operationRequests/filter', URL_OperationRequests),
    setup_call_cleanup(
        http_open(URL_OperationRequests, In, [cert_verify_hook(cert_accept_any)]),
        (read_string(In, _, Response),
         atom_json_dict(Response, OperationRequests, [])),
        close(In)).

% Convert OperationRequest to assignment_surgery
convert_operation_request_to_surgery_id(OperationRequestDict, assignment_surgery(Id, DoctorId)) :-
    Id = OperationRequestDict.get(id),
    DoctorId = OperationRequestDict.get(doctorId).

% Convert assignment_surgery to JSON
convert_assignment_surgery_to_json(assignment_surgery(Id, DoctorId), json([id = Id, doctorId = DoctorId])).

assignment_surgery(Id, DoctorId) :- cconvert_operation_request_to_surgery_id(_, assignment_surgery(Id, DoctorId)).

%-------------------------OperationTypes - Surgeries--------------------------%
:- http_handler(api(surgeries), get_surgeries, []).

% GET OperationTypes - Surgeries
get_surgeries(_Request):-
    get_operation_types_backend(OperationTypes),
    maplist(convert_operation_type_to_surgery, OperationTypes, Surgeries),
    maplist(convert_surgery_to_json, Surgeries, JSON_Surgeries),
    reply_json(json([surgeries=JSON_Surgeries]), [json_object(dict)]).

% Convert OperationType to surgery_id
convert_operation_type_to_surgery(OperationTypeDict, surgery(Name, TAnesthesia,TSurgery,TCleaning)) :-
    Name = OperationTypeDict.get(name),
    TAnesthesia = OperationTypeDict.get(anesthesiaTime),
    TSurgery = OperationTypeDict.get(surgeryTime),
    TCleaning = OperationTypeDict.get(cleaningTime).

% Convert surgery to JSON
convert_surgery_to_json(surgery(Name, TAnesthesia, TSurgery, TCleaning), json([name=Name, tAnesthesia=TAnesthesia, tSurgery=TSurgery, tCleaning=TCleaning])).

surgery(Name, TAnesthesia,TSurgery,TCleaning) :- convert_operation_type_to_surgery(_, surgery(Name, TAnesthesia,TSurgery,TCleaning)).

%-------------------------------Staffs---------------------------------%
:- http_handler(api(staffs), get_staffs, []).

% GET Staffs
get_staffs(_Request) :-
    get_operation_types_backend(OperationTypes),
    get_staffs_backend(Staffs),
    maplist(convert_staff_operation_to_staff(OperationTypes), Staffs, StaffWithOperations),
    maplist(convert_staff_to_json, StaffWithOperations, StaffJsons),
    reply_json(json([staffs = StaffJsons]), [json_object(dict)]).

% GET Staffs Backend
get_staffs_backend(Staffs) :-
    backend_url(URL),
    atom_concat(URL, 'staffs/filter', URL_Staffs),
    http_open(URL_Staffs, In, [cert_verify_hook(cert_accept_any)]),
    read_string(In, _, Response),
    atom_json_dict(Response, Staffs, []),
    close(In).

% Convert Staff with operation to staff

convert_staff_operation_to_staff(OperationTypes, StaffDict, staff(StaffId, StaffRole, SpecializationName, MatchingOperations)) :-
    StaffId = StaffDict.get(id),
    StaffUser = StaffDict.get(user),
    StaffRole = StaffUser.get(role),
    Specialization = StaffDict.get(specialization),
    SpecializationName = Specialization.get(name),  

    findall(OperationId, 
            (member(OperationType, OperationTypes),         
            StaffSpecializations = OperationType.get(staffSpecializationDtos),  
            
            member(StaffSpecialization, StaffSpecializations),
            SpecializationName = StaffSpecialization.get(specializationName),  
            OperationId = OperationType.get(id)
        ),
        MatchingOperations  
    ).

% Convert staff to JSON
convert_staff_to_json(staff(StaffId, StaffRole, SpecializationName, MatchingOperations), 
                      json([id = StaffId, role = StaffRole, specialization = SpecializationName, operations = MatchingOperations])).

:- server(4000).