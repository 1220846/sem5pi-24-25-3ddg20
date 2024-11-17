import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalEditStaffProfileComponent } from './modal-edit-staff-profile.component';

describe('ModalEditStaffProfileComponent', () => {
  let component: ModalEditStaffProfileComponent;
  let fixture: ComponentFixture<ModalEditStaffProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalEditStaffProfileComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalEditStaffProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
