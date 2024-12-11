import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchMedicalConditionsComponent } from './search-medical-conditions.component';

describe('SearchMedicalConditionsComponent', () => {
  let component: SearchMedicalConditionsComponent;
  let fixture: ComponentFixture<SearchMedicalConditionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchMedicalConditionsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchMedicalConditionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
