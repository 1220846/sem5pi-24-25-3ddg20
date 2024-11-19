import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalCreateOperationRequestComponent } from './modal-create-operation-request.component';

describe('ModalCreateOperationRequestComponent', () => {
  let component: ModalCreateOperationRequestComponent;
  let fixture: ComponentFixture<ModalCreateOperationRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalCreateOperationRequestComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalCreateOperationRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
