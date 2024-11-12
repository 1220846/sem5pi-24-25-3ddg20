import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateUserPatientComponent } from './update-user-patient.component';

describe('UpdateUserPatientComponent', () => {
  let component: UpdateUserPatientComponent;
  let fixture: ComponentFixture<UpdateUserPatientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateUserPatientComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateUserPatientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
