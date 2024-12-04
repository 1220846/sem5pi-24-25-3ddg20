import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalCreateSpecializationComponent } from './modal-create-specialization.component';

describe('ModalCreateSpecializationComponent', () => {
  let component: ModalCreateSpecializationComponent;
  let fixture: ComponentFixture<ModalCreateSpecializationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalCreateSpecializationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalCreateSpecializationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
