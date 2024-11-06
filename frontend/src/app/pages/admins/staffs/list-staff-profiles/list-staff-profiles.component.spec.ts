import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListStaffProfilesComponent } from './list-staff-profiles.component';

describe('ListStaffProfilesComponent', () => {
  let component: ListStaffProfilesComponent;
  let fixture: ComponentFixture<ListStaffProfilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListStaffProfilesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListStaffProfilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
