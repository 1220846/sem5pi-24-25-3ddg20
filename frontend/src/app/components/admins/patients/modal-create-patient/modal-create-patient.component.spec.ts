import { ComponentFixture, fakeAsync, flush, TestBed } from '@angular/core/testing';

import { ModalCreatePatientComponent } from './modal-create-patient.component';
import { CreatingPatientDto } from '../../../../domain/CreatingPatientDto';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { PatientService } from '../../../../services/patient.service';
import { of, throwError } from 'rxjs';
import { Patient } from '../../../../domain/Patient';

describe('ModalCreatePatientComponent', () => {
  let component: ModalCreatePatientComponent;
  let fixture: ComponentFixture<ModalCreatePatientComponent>;
  let patientServiceMock: jasmine.SpyObj<PatientService>;

  beforeEach(async () => {
    patientServiceMock = jasmine.createSpyObj('PatientService', ['add']);

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, ModalCreatePatientComponent],
      providers: [
        FormBuilder,
        { provide: PatientService, useValue: patientServiceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ModalCreatePatientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  describe('showDialog', () => {
    it('should reset the form and set dialog visibility to true', () => {
      component.patientForm.setValue({
        firstName: 'John',
        lastName: 'Doe',
        fullName: 'John Doe',
        dateOfBirth: '01/01/1991',
        gender: 'Male',
        emergencyContact: '912345678',
        phoneNumber: '912345678',
        email: 'john.doe@example.com',
        address: '123 Street Name',
        postalCode: '1234-567'
      });

      component.showDialog();

      expect(component.patientForm.pristine).toBeTrue();
      expect(component.patientForm.valid).toBeFalse(); // Because form is reset
      expect(component.visible).toBeTrue();
    });
  });

  describe('saveData', () => {
    it('should call patientService.add with valid data when form is valid', fakeAsync(() => {
      const mockCreatePatient: CreatingPatientDto = {
        firstName: 'John',
        lastName: 'Doe',
        fullName: 'John Doe',
        dateOfBirth: '01/01/1991',
        gender: 'Male',
        emergencyContact: '912345678',
        phoneNumber: '912345678',
        email: 'john.doe@example.com',
        address: '123 Street Name',
        postalCode: '1234-567'
      };

      const mockPatient: Patient = {
        id: '10102024000021',
        firstName: 'John',
        lastName: 'Doe',
        fullName: 'John Doe',
        dateOfBirth: '01/01/1991',
        gender: 'Male',
        emergencyContact: '912345678',
        phoneNumber: '912345678',
        email: 'john.doe@example.com',
        address: '123 Street Name',
        postalCode: '1234-567',
        medicalConditions: 'a'
      };

      component.patientForm.setValue(mockCreatePatient);
      patientServiceMock.add.and.returnValue(of(mockPatient));

      spyOn(component.patientCreated, 'emit');
      component.saveData();

      expect(patientServiceMock.add).toHaveBeenCalledWith(mockCreatePatient);
      expect(component.patientCreated.emit).toHaveBeenCalledWith(mockCreatePatient);
      expect(component.visible).toBeFalse();
      expect(component.patientForm.pristine).toBeTrue();

      flush();
    }));

    it('should not call patientService.add if form is invalid', () => {
      component.patientForm.setValue({
        firstName: '',
        lastName: '',
        fullName: '',
        dateOfBirth: '',
        gender: '',
        emergencyContact: '',
        phoneNumber: '',
        email: '',
        address: '',
        postalCode: ''
      });

      spyOn(console, 'warn');
      component.saveData();

      expect(patientServiceMock.add).not.toHaveBeenCalled();
      expect(console.warn).toHaveBeenCalledWith('Form is invalid!');
    });

    it('should handle service errors gracefully', fakeAsync(() => {
      const mockPatient: CreatingPatientDto = {
        firstName: 'John',
        lastName: 'Doe',
        fullName: 'John Doe',
        dateOfBirth: '01/01/1991',
        gender: 'Male',
        emergencyContact: '912345678',
        phoneNumber: '912345678',
        email: 'john.doe@example.com',
        address: '123 Street Name',
        postalCode: '1234-567'
      };

      component.patientForm.setValue(mockPatient);
      patientServiceMock.add.and.returnValue(throwError(() => new Error('API Error')));

      spyOn(console, 'error');
      component.saveData();

      expect(patientServiceMock.add).toHaveBeenCalledWith(mockPatient);
      expect(console.error).toHaveBeenCalledWith('Error adding patient:', jasmine.any(Error));

      flush();
    }));
  });

  describe('Form Validation', () => {
    it('should mark all controls as invalid when empty', () => {
      component.patientForm.setValue({
        firstName: '',
        lastName: '',
        fullName: '',
        dateOfBirth: '',
        gender: '',
        emergencyContact: '',
        phoneNumber: '',
        email: '',
        address: '',
        postalCode: ''
      });

      expect(component.patientForm.valid).toBeFalse();
      expect(component.patientForm.controls['firstName'].hasError('required')).toBeTrue();
      expect(component.patientForm.controls['email'].hasError('email')).toBeFalse();
    });

    it('should validate phone number and postal code patterns', () => {
      component.patientForm.setValue({
        firstName: 'John',
        lastName: 'Doe',
        fullName: 'John Doe',
        dateOfBirth: '01/01/1991',
        gender: 'Male',
        emergencyContact: '123456789', // Invalid
        phoneNumber: '123456789', // Invalid
        email: 'invalid-email',
        address: '123 Street Name',
        postalCode: '123' // Invalid
      });

      expect(component.patientForm.valid).toBeFalse();
      expect(component.patientForm.controls['emergencyContact'].hasError('pattern')).toBeTrue();
      expect(component.patientForm.controls['phoneNumber'].hasError('pattern')).toBeTrue();
      expect(component.patientForm.controls['postalCode'].hasError('pattern')).toBeTrue();
      expect(component.patientForm.controls['email'].hasError('email')).toBeTrue();
    });
  });
});
