import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalEditOperationTypeComponent } from './modal-edit-operation-type.component';

describe('ModalEditOperationTypeComponent', () => {
  let component: ModalEditOperationTypeComponent;
  let fixture: ComponentFixture<ModalEditOperationTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalEditOperationTypeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalEditOperationTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
