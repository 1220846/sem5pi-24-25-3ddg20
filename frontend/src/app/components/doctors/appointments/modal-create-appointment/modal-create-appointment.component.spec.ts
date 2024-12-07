import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalCreateAppointmentComponent } from './modal-create-appointment.component';

describe('ModalCreateAppointmentComponent', () => {
  let component: ModalCreateAppointmentComponent;
  let fixture: ComponentFixture<ModalCreateAppointmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalCreateAppointmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalCreateAppointmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
