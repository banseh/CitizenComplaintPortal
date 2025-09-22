import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComplaintCategoryComponent } from './complaint-category.component';

describe('ComplaintCategoryComponent', () => {
  let component: ComplaintCategoryComponent;
  let fixture: ComponentFixture<ComplaintCategoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComplaintCategoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ComplaintCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
