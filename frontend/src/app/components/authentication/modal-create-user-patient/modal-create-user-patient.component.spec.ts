import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalCreateUserPatientComponent } from './modal-create-user-patient.component';

describe('ModalCreateUserPatientComponent', () => {
  let component: ModalCreateUserPatientComponent;
  let fixture: ComponentFixture<ModalCreateUserPatientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalCreateUserPatientComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalCreateUserPatientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
