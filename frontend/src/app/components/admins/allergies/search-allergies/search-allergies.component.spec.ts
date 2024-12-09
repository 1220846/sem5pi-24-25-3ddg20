import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchAllergiesComponent } from './search-allergies.component';

describe('SearchAllergiesComponent', () => {
  let component: SearchAllergiesComponent;
  let fixture: ComponentFixture<SearchAllergiesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchAllergiesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchAllergiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
