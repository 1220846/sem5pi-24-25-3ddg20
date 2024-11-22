import { ComponentFixture, fakeAsync, flush, TestBed } from '@angular/core/testing';
import { ModalCreateOperationTypeComponent } from './modal-create-operation-type.component';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { SpecializationService } from '../../../../services/specialization.service';
import { OperationTypeService } from '../../../../services/operation-type.service';
import { MessageService } from 'primeng/api';
import { OperationType } from '../../../../domain/OperationType';

describe('ModalCreateOperationTypeComponent', () => {
  let component: ModalCreateOperationTypeComponent;
  let fixture: ComponentFixture<ModalCreateOperationTypeComponent>;
  let specializationServiceMock: jasmine.SpyObj<SpecializationService>;
  let operationTypeServiceMock: jasmine.SpyObj<OperationTypeService>;
  let messageServiceMock: jasmine.SpyObj<MessageService>;

  beforeEach(async () => {
    specializationServiceMock = jasmine.createSpyObj('SpecializationService', ['getAll']);
    operationTypeServiceMock = jasmine.createSpyObj('OperationTypeService', ['add']);
    messageServiceMock = jasmine.createSpyObj('MessageService', ['add']);

    await TestBed.configureTestingModule({
      imports: [ModalCreateOperationTypeComponent, ReactiveFormsModule],
      providers: [
        FormBuilder,
        { provide: SpecializationService, useValue: specializationServiceMock },
        { provide: OperationTypeService, useValue: operationTypeServiceMock },
        { provide: MessageService, useValue: messageServiceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ModalCreateOperationTypeComponent);
    component = fixture.componentInstance;

    // Mock specializations data
    specializationServiceMock.getAll.and.returnValue(of([
      { id: '1', name: 'Cardiology' },
      { id: '2', name: 'Neurology' }
    ]));

    fixture.detectChanges(); // Trigger ngOnInit
  });

  // Test initialization
  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  describe('ngOnInit', () => {
    it('should load specializations on init', () => {
      component.ngOnInit();
      expect(specializationServiceMock.getAll).toHaveBeenCalled();
      expect(component.specializations.length).toBe(2);
    });
  });

  describe('showDialog', () => {
    it('should reset form and selected specializations when dialog is shown', () => {
      component.operationTypeForm.patchValue({ name: 'Test' });
      component.selectedSpecializations.push({
        specialization: { id: '1', name: 'Cardiology' },
        numberOfStaff: 2
      });
      component.showDialog();

      expect(component.operationTypeForm.get('name')?.value).toBeNull();
      expect(component.selectedSpecializations.length).toBe(0);
      expect(component.visible).toBeTrue();
    });
  });

  describe('addSpecialization', () => {
    it('should add a selected specialization to the list', () => {
      component.operationTypeForm.get('selectedSpecialization')?.setValue({ id: 1, name: 'Cardiology' });

      component.addSpecialization();

      expect(component.selectedSpecializations.length).toBe(1); 
      expect(component.selectedSpecializations[0].specialization.name).toBe('Cardiology');
    });

    it('should not add a specialization already in the list', () => {
      component.selectedSpecializations.push({
        specialization: { id: '1', name: 'Cardiology' },
        numberOfStaff: 1
      });

      component.operationTypeForm.get('selectedSpecialization')?.setValue({ id: '1', name: 'Cardiology' });
      component.addSpecialization();

      expect(component.selectedSpecializations.length).toBe(1); 
      expect(component.selectedSpecializations[0].specialization.name).toBe('Cardiology');
    });
  });

  describe('removeSpecialization', () => {
    it('should remove a specialization from the list', () => {
      component.selectedSpecializations = [
        { specialization: { id: '1', name: 'Cardiology' }, numberOfStaff: 1 },
        { specialization: { id: '2', name: 'Neurology' }, numberOfStaff: 1 }
      ];

      component.removeSpecialization(component.selectedSpecializations[0]);

      expect(component.selectedSpecializations.length).toBe(1);
      expect(component.selectedSpecializations[0].specialization.name).toBe('Neurology');
    });
  });

  it('should call service to save data when form is valid', fakeAsync(() => {
    component.operationTypeForm.setValue({
      name: 'Operation Test',
      surgeryTime: 60,
      anesthesiaTime: 30,
      cleaningTime: 15,
      selectedSpecialization:[
        { specialization: { id: '1', name: 'Cardiology' }, numberOfStaff: 2 }
      ]
    });

    operationTypeServiceMock.add.and.returnValue(of({
      id: '1',
      name: 'Operation Test',
      estimatedDuration: 105,
      surgeryTime: 60,
      anesthesiaTime: 30,
      cleaningTime: 15,
      operationTypeStatus: 'Active',
      staffSpecializationDtos: [
        { specializationId: '1', specializationName: 'Cardiology', numberOfStaff: 2 }
      ]
    }));

    component.saveData();

    expect(operationTypeServiceMock.add).toHaveBeenCalledWith(jasmine.objectContaining({
      name: 'Operation Test',
      estimatedDuration: 105,
      surgeryTime: 60,
      anesthesiaTime: 30,
      cleaningTime: 15,
    }));

    expect(component.visible).toBeFalse();
  }));

  it('should show warning if form is invalid', fakeAsync(() => {
    spyOn(console, 'warn');
    component.operationTypeForm.reset(); 
    component.saveData();
    expect(operationTypeServiceMock.add).not.toHaveBeenCalled();
  }));
});