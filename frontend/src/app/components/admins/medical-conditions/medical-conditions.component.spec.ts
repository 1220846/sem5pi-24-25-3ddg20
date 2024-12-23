import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MedicalConditionsComponent } from './medical-conditions.component';

describe('MedicalConditionsComponent', () => {
  let component: MedicalConditionsComponent;
  let fixture: ComponentFixture<MedicalConditionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MedicalConditionsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MedicalConditionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
