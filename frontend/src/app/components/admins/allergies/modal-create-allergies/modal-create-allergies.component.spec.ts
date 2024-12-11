import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalCreateAllergiesComponent } from './modal-create-allergies.component';

describe('ModalCreateAllergiesComponent', () => {
  let component: ModalCreateAllergiesComponent;
  let fixture: ComponentFixture<ModalCreateAllergiesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalCreateAllergiesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalCreateAllergiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
