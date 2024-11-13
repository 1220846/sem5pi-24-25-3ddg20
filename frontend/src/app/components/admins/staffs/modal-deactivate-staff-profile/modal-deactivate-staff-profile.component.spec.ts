import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalDeactivateStaffProfileComponent } from './modal-deactivate-staff-profile.component';

describe('ModalDeactivateStaffProfileComponent', () => {
  let component: ModalDeactivateStaffProfileComponent;
  let fixture: ComponentFixture<ModalDeactivateStaffProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalDeactivateStaffProfileComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalDeactivateStaffProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
