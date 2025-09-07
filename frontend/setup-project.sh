# ========================================
# ANGULAR PROJECT SETUP COMMANDS
# AI Travel Planner - Scalable Structure
# (Modified for existing frontend folder)
# ========================================

echo "Setting up project structure in current directory..."
echo "Current directory: $(pwd)"

# Step 1: Create Core Directory Structure
mkdir -p src/app/core/{guards,interceptors,services,constants,config}
mkdir -p src/app/core/services/{api,auth,storage,notification}

# Step 2: Create Shared Directory Structure
mkdir -p src/app/shared/{components,pipes,directives,validators,utils}
mkdir -p src/app/shared/components/{ui,forms,layout}

# Step 3: Create Features Directory Structure
mkdir -p src/app/features/{auth,travel,dashboard,profile}
mkdir -p src/app/features/auth/{components,services}
mkdir -p src/app/features/travel/{components,services,models}
mkdir -p src/app/features/dashboard/{components,services}
mkdir -p src/app/features/profile/{components,services}

# Step 4: Create Layout Directory Structure
mkdir -p src/app/layout/{main-layout,auth-layout,admin-layout,error-layout}

# Step 5: Create Models Directory Structure
mkdir -p src/app/models/{api,user,common}

# Step 6: Create State Directory Structure (if using NgRx)
mkdir -p src/app/state/{auth,travel}

# Step 7: Create Assets Directory Structure
mkdir -p src/assets/{images,styles,i18n,data}
mkdir -p src/assets/images/{destinations,icons,logos,backgrounds}
mkdir -p src/assets/styles/{abstracts,base,components,layout,pages,themes,vendors}

# Step 8: Create Documentation Directory
mkdir -p docs

echo "‚úÖ Directory structure created!"

# ========================================
# GENERATE CORE SERVICES
# ========================================

echo "Generating core services..."

# Core Guards
ng generate guard core/guards/auth --implements CanActivate --standalone
ng generate guard core/guards/role --implements CanActivate --standalone
ng generate guard core/guards/unsaved-changes --implements CanDeactivate --standalone

# Core Interceptors
ng generate interceptor core/interceptors/auth --standalone
ng generate interceptor core/interceptors/error --standalone
ng generate interceptor core/interceptors/loading --standalone

# Core Services
ng generate service core/services/api/api --standalone
ng generate service core/services/api/api-config --standalone
ng generate service core/services/auth/auth --standalone
ng generate service core/services/auth/token --standalone
ng generate service core/services/auth/user --standalone
ng generate service core/services/storage/local-storage --standalone
ng generate service core/services/storage/session-storage --standalone
ng generate service core/services/notification/toast --standalone
ng generate service core/services/notification/notification --standalone

echo "‚úÖ Core services generated!"

# ========================================
# GENERATE SHARED COMPONENTS
# ========================================

echo "Generating shared components..."

# UI Components
ng generate component shared/components/ui/button --standalone
ng generate component shared/components/ui/modal --standalone
ng generate component shared/components/ui/loading-spinner --standalone
ng generate component shared/components/ui/pagination --standalone
ng generate component shared/components/ui/card --standalone

# Form Components
ng generate component shared/components/forms/date-picker --standalone
ng generate component shared/components/forms/multi-select --standalone
ng generate component shared/components/forms/search-input --standalone

# Layout Components
ng generate component shared/components/layout/header --standalone
ng generate component shared/components/layout/footer --standalone
ng generate component shared/components/layout/sidebar --standalone
ng generate component shared/components/layout/breadcrumb --standalone

# Shared Pipes
ng generate pipe shared/pipes/currency-format --standalone
ng generate pipe shared/pipes/date-format --standalone
ng generate pipe shared/pipes/truncate --standalone

# Shared Directives
ng generate directive shared/directives/click-outside --standalone
ng generate directive shared/directives/auto-focus --standalone

echo "‚úÖ Shared components generated!"

# ========================================
# GENERATE LAYOUT COMPONENTS
# ========================================

echo "Generating layout components..."

ng generate component layout/main-layout --standalone
ng generate component layout/auth-layout --standalone

echo "‚úÖ Layout components generated!"

# ========================================
# GENERATE FEATURE COMPONENTS - TRAVEL
# ========================================

echo "Generating travel feature components..."

ng generate component features/travel/components/plan-trip --standalone
ng generate component features/travel/components/trip-form --standalone
ng generate component features/travel/components/destination-selector --standalone
ng generate component features/travel/components/itinerary-view --standalone
ng generate component features/travel/components/trip-card --standalone

ng generate service features/travel/services/travel-planner --standalone
ng generate service features/travel/services/destination --standalone

echo "‚úÖ Travel feature components generated!"

# ========================================
# CREATE CONFIGURATION FILES
# ========================================

echo "Creating configuration files..."

# Create Constants Files
touch src/app/core/constants/app-constants.ts
touch src/app/core/constants/api-endpoints.ts
touch src/app/core/constants/storage-keys.ts

# Create Model Files
touch src/app/models/api/api-response.model.ts
touch src/app/models/user/user.model.ts
touch src/app/features/travel/models/travel-plan.model.ts
touch src/app/features/travel/models/destination.model.ts

# Create Route Files
touch src/app/features/travel/travel.routes.ts

# Create Style Files
touch src/assets/styles/abstracts/_variables.scss
touch src/assets/styles/abstracts/_mixins.scss
touch src/assets/styles/base/_base.scss

# Create Barrel Export Files
touch src/app/shared/components/index.ts
touch src/app/core/services/index.ts

echo "‚úÖ Configuration files created!"

echo ""
echo "Ìæâ PROJECT SETUP COMPLETE!"
echo "Ì≥Å Scalable Angular structure created successfully!"
echo ""
echo "Next steps:"
echo "1. Update tsconfig.json with path mappings"
echo "2. Configure environment files"
echo "3. Set up routing in app.routes.ts"
echo "4. Run 'ng serve' to start development server"
