import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalRemoveSpecializationComponent } from './modal-remove-specialization.component';

describe('ModalRemoveSpecializationComponent', () => {
  let component: ModalRemoveSpecializationComponent;
  let fixture: ComponentFixture<ModalRemoveSpecializationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalRemoveSpecializationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalRemoveSpecializationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
