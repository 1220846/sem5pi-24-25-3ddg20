import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalDeleteOperationTypeComponent } from './modal-delete-operation-type.component';

describe('ModalDeleteOperationTypeComponent', () => {
  let component: ModalDeleteOperationTypeComponent;
  let fixture: ComponentFixture<ModalDeleteOperationTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalDeleteOperationTypeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalDeleteOperationTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
