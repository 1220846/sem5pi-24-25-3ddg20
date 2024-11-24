import { ComponentFixture, fakeAsync, TestBed } from '@angular/core/testing';
import { ModalUpdateOperationRequestsComponent } from './modal-update-operation-requests.component';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { OperationRequestService } from '../../../../services/operation-request.service';
import { MessageService } from 'primeng/api';

describe('ModalUpdateOperationRequestsComponent', () => {
  let component: ModalUpdateOperationRequestsComponent;
  let fixture: ComponentFixture<ModalUpdateOperationRequestsComponent>;
  let operationRequestServiceMock: jasmine.SpyObj<OperationRequestService>;
  let messageServiceMock: jasmine.SpyObj<MessageService>;

  beforeEach(async () => {
    operationRequestServiceMock = jasmine.createSpyObj('OperationRequestService', ['edit']);
    messageServiceMock = jasmine.createSpyObj('MessageService', ['add']);

    await TestBed.configureTestingModule({
      imports: [ModalUpdateOperationRequestsComponent, ReactiveFormsModule],
      providers: [
        FormBuilder,
        { provide: OperationRequestService, useValue: operationRequestServiceMock },
        { provide: MessageService, useValue: messageServiceMock },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ModalUpdateOperationRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  describe('ngOnInit', () => {
    it('should initialize the form', () => {
      expect(component.operationRequestForm).toBeTruthy();
      expect(component.operationRequestForm.get('selectedPriority')).toBeDefined();
      expect(component.operationRequestForm.get('deadline')).toBeDefined();
    });
  });

  describe('showDialog', () => {
    it('should make the dialog visible', () => {
      component.showDialog();
      expect(component.visible).toBeTrue();
    });
  });

  describe('changeInfo', () => {
    it('should emit success message and event if form is valid and API call succeeds', fakeAsync(() => {
      component.operationRequest = {
        id: '123',
        doctorId: '456',
        operationTypeId: '789',
        operationTypeName: 'Surgery',
        medicalRecordNumber: 'MRN001',
        deadline: '2024-12-25',
        priority: 'ELECTIVE',
        status: 'Pending',
      };

      component.operationRequestForm.setValue({
        selectedPriority: 'EMERGENCY',
        deadline: '25/12/2024',
      });

      operationRequestServiceMock.edit.and.returnValue(
        of({
          id: '123',
          doctorId: '456',
          operationTypeId: '789',
          operationTypeName: 'Surgery',
          medicalRecordNumber: 'MRN001',
          deadline: '25/12/2024',
          priority: 'EMERGENCY',
          status: 'Pending',
        })
      );
      

      const spyEmit = spyOn(component.operationRequestEdited, 'emit');

      component.changeInfo();

      expect(operationRequestServiceMock.edit).toHaveBeenCalledWith('123', {
        deadline: '25/12/2024',
        priority: 'EMERGENCY',
      });
      expect(spyEmit).toHaveBeenCalled();
      expect(component.visible).toBeFalse();
    }));

    it('should show error message if API call fails', fakeAsync(() => {
      component.operationRequest = {
        id: '123',
        doctorId: '456',
        operationTypeId: '789',
        operationTypeName: 'Surgery',
        medicalRecordNumber: 'MRN001',
        deadline: '25/12/2024',
        priority: 'ELECTIVE',
        status: 'Pending',
      };

      component.operationRequestForm.setValue({
        selectedPriority: 'EMERGENCY',
        deadline: '25/12/2024',
      });

      operationRequestServiceMock.edit.and.returnValue(throwError(() => new Error('Error')));

      component.changeInfo();

      expect(operationRequestServiceMock.edit).toHaveBeenCalledWith('123', {
        deadline: '25/12/2024', 
        priority: 'EMERGENCY',
      });
      expect(component.visible).toBeFalse();
    }));

    it('should show warning if form is invalid', () => {
      component.operationRequestForm.reset();

      component.changeInfo();

      expect(operationRequestServiceMock.edit).not.toHaveBeenCalled();
    });
  });

  describe('closeDialog', () => {
    it('should hide the dialog', () => {
      component.visible = true;
      component.closeDialog();
      expect(component.visible).toBeFalse();
    });
  });
});
