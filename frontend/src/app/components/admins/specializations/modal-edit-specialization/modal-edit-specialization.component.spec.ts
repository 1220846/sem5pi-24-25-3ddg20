import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalEditSpecializationComponent } from './modal-edit-specialization.component';

describe('ModalEditSpecializationComponent', () => {
  let component: ModalEditSpecializationComponent;
  let fixture: ComponentFixture<ModalEditSpecializationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalEditSpecializationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalEditSpecializationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
