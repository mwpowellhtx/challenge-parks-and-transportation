#pragma once

namespace challenges {

    template<class Derived>
    class cloneable {

    protected:

        cloneable() {}

        cloneable(cloneable const &) {}

    public:

        typedef Derived derived_type;

        virtual ~cloneable() {}

        virtual derived_type clone() const abstract;
    };
}
