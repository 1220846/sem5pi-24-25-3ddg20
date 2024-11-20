import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalUpdatePatientComponent } from './modal-update-patient.component';

describe('ModalUpdatePatientComponent', () => {
  let component: ModalUpdatePatientComponent;
  let fixture: ComponentFixture<ModalUpdatePatientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalUpdatePatientComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalUpdatePatientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
