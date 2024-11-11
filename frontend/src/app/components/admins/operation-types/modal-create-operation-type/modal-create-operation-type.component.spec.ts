import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalCreateOperationTypeComponent } from './modal-create-operation-type.component';

describe('ModalCreateOperationTypeComponent', () => {
  let component: ModalCreateOperationTypeComponent;
  let fixture: ComponentFixture<ModalCreateOperationTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalCreateOperationTypeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalCreateOperationTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
