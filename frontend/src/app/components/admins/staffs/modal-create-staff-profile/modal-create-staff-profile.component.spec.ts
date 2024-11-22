import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CreateStaffProfileComponent } from './modal-create-staff-profile.component';
import { FormBuilder } from '@angular/forms';
import { StaffService } from '../../../../services/staff.service';
import { SpecializationService } from '../../../../services/specialization.service';
import { of, throwError } from 'rxjs';
import { MessageService } from 'primeng/api';

describe('CreateStaffProfileComponent', () => {
  let component: CreateStaffProfileComponent;
  let fixture: ComponentFixture<CreateStaffProfileComponent>;
  let mockStaffService: jasmine.SpyObj<StaffService>;
  let mockSpecializationService: jasmine.SpyObj<SpecializationService>;
  let mockMessageService: jasmine.SpyObj<MessageService>;

  beforeEach(async () => {
    mockStaffService = jasmine.createSpyObj('StaffService', ['add']);
    mockSpecializationService = jasmine.createSpyObj('SpecializationService', ['getAll']);
    mockMessageService = jasmine.createSpyObj('MessageService', ['add']);

    await TestBed.configureTestingModule({
      imports: [CreateStaffProfileComponent],
      providers: [
        FormBuilder,
        { provide: StaffService, useValue: mockStaffService },
        { provide: SpecializationService, useValue: mockSpecializationService },
        { provide: MessageService, useValue: mockMessageService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(CreateStaffProfileComponent);
    component = fixture.componentInstance;

    // Mocking the return of `getAll` to load specializations
    mockSpecializationService.getAll.and.returnValue(of([{ id: '1', name: 'Anaesthetist' }]));

    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form correctly', () => {
    expect(component.staffForm).toBeTruthy();
    expect(component.staffForm.controls['firstName']).toBeTruthy();
    expect(component.staffForm.controls['lastName']).toBeTruthy();
    expect(component.staffForm.controls['specialization']).toBeTruthy();
  });

  it('should load specializations on initialization', () => {
    component.ngOnInit();
    expect(component.specializations.length).toBe(1);
    expect(component.specializations[0].name).toBe('Anaesthetist');
  });

  it('should display the modal when calling showDialog', () => {
    component.showDialog();
    expect(component.visible).toBeTrue();
    expect(component.staffForm.valid).toBeFalse(); // The form should be empty/invalid
  });

  it('should save data correctly when the form is valid', () => {
    // Setting up a valid form
    component.staffForm.setValue({
      firstName: 'John',
      lastName: 'Doe',
      fullName: 'John Doe',
      email: 'john.doe@example.com',
      phoneNumber: '123456789',
      licenseNumber: 'D123456',
      specialization: { id: '1', name: 'Anaesthetist' },
      userEmail: 'O202499999',
    });

    mockStaffService.add.and.returnValue(of({
      id: 'O202499999',
      firstName: 'John',
      lastName: 'Doe',
      fullName: 'John Doe',
      email: 'john.doe@example.com',
      phoneNumber: '123456789',
      licenseNumber: 'D123456',
      specializationId: '1',
      userEmail: 'O202499999@sarm.com',
      status: 'ACTIVE',
      availabilitySlots: [], // Can be an empty array or an appropriate value for the test
      specialization: { id: '1', name: 'Anaesthetist' }, // Add specialization here
      user: { username: 'O202499999', email: 'john.doe@example.com', role: 'Doctor' } // Add user here
    }));

    component.saveData();

    expect(mockStaffService.add).toHaveBeenCalledWith(jasmine.objectContaining({
      firstName: 'John',
      lastName: 'Doe',
    }));
    expect(component.visible).toBeFalse();

  });

  it('should display an error when the form is invalid', () => {
    spyOn(console, 'warn');
    component.staffForm.reset(); // The form will be invalid
    component.saveData();
    expect(mockStaffService.add).not.toHaveBeenCalled();
    expect(console.warn).toHaveBeenCalledWith('Form is invalid!');
  });
});
