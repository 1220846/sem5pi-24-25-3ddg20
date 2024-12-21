import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalUpdateAllergyComponent } from './modal-update-allergy.component';

describe('ModalUpdateAllergyComponent', () => {
  let component: ModalUpdateAllergyComponent;
  let fixture: ComponentFixture<ModalUpdateAllergyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalUpdateAllergyComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalUpdateAllergyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
