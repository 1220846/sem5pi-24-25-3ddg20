import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalCreateMedicalConditionComponent } from './modal-create-medical-condition.component';

describe('ModalCreateMedicalConditionComponent', () => {
  let component: ModalCreateMedicalConditionComponent;
  let fixture: ComponentFixture<ModalCreateMedicalConditionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalCreateMedicalConditionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalCreateMedicalConditionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
