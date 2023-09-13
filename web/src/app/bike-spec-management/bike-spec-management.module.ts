import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { SpecificationComponent } from './components/specification/specification.component';
import { AttributeListComponent } from './components/attribute/attribute-list/attribute-list.component';
import { AttributeComponent } from './components/attribute/attribute.component';
import { SpecificationListComponent } from './components/specification/specification-list/specification-list.component';
import { BikeSpecificationService } from './services/bike-specification.service';
import { BrandListComponent } from './components/brand-list/brand-list.component';
import { BudgetListComponent } from './components/budget-list/budget-list.component';
import { BudgetComponent } from './components/budget-list/budget/budget.component';
import { DisplacementListComponent } from './components/displacement-list/displacement-list.component';
import { DisplacementComponent } from './components/displacement-list/displacement/displacement.component';
import { BodyStyleListComponent } from './components/body-style-list/body-style-list.component';
import { BodyStyleComponent } from './components/body-style-list/body-style/body-style.component';
import { ColourListComponent } from './components/colour-list/colour-list.component';
import { ColourComponent } from './components/colour-list/colour/colour.component';
import { CategoryComponent } from './components/category/category.component';
import { CategoryListComponent } from './components/category-list/category-list.component';
import { BrandComponent } from './components/brand-list/brand/brand.component';

export const routes: Routes = [
  { path: 'specification', component: SpecificationListComponent},
  { path: 'attribute', component: AttributeListComponent },
  { path: 'brands', component: BrandListComponent },
  { path: 'body-style', component: BodyStyleListComponent },
  { path: 'displacement', component: DisplacementListComponent },
  { path: 'budget', component: BudgetListComponent },
  { path: 'category', component: CategoryListComponent },
  { path: 'colour', component: ColourListComponent },
];

@NgModule({
  declarations: [SpecificationComponent, AttributeComponent,BrandComponent,
     AttributeListComponent, SpecificationListComponent, BrandListComponent, BudgetListComponent, BudgetComponent, DisplacementListComponent, DisplacementComponent, BodyStyleListComponent, BodyStyleComponent, ColourListComponent, ColourComponent, CategoryComponent, CategoryListComponent],
  providers:[BikeSpecificationService],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes)
  ]
})
export class BikeSpecManagementModule { }
