#ifndef PARKPLANNER_CHALLENGE_H
#define PARKPLANNER_CHALLENGE_H

#include "theme_park.h"

#include <common\challenge_base.hpp>

namespace challenges {

    namespace park {

        class challenge : public challenge_base {
        private:

            std::shared_ptr<theme_park> _sp_theme_park;

        public:

            challenge(std::istream * pis, std::ostream * pos);

            ~challenge();

            void read(std::istream & is);

            virtual void run();

            virtual void report(std::ostream & os);
        };
    }
}

#endif //PARKPLANNER_CHALLENGE_H
