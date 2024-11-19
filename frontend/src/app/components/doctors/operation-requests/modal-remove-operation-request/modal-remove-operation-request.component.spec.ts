import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalRemoveOperationRequestComponent } from './modal-remove-operation-request.component';

describe('ModalRemoveOperationRequestComponent', () => {
  let component: ModalRemoveOperationRequestComponent;
  let fixture: ComponentFixture<ModalRemoveOperationRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalRemoveOperationRequestComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalRemoveOperationRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
