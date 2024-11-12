import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalCreatePatientComponent } from './modal-create-patient.component';

describe('ModalCreatePatientComponent', () => {
  let component: ModalCreatePatientComponent;
  let fixture: ComponentFixture<ModalCreatePatientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalCreatePatientComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalCreatePatientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
